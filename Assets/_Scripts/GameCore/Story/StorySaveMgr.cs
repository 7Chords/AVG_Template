using GameCore.RefData;
using SCFrame;
using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Story
{
    /// <summary>
    /// 多槽位剧情存档，JSON 写入 persistentDataPath/StorySaves。
    /// </summary>
    public static class StorySaveMgr
    {
        public const int MaxSlotCount = 12;
        private const string SavesSubFolder = "StorySaves";

        public static string GetSlotFilePath(int _slotIndex)
        {
            return Path.Combine(GetSavesDirectory(), $"slot_{_slotIndex}.json");
        }

        public static string GetSavesDirectory()
        {
            string dir = Path.Combine(Application.persistentDataPath, SavesSubFolder);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

        public static StorySaveSlotData ReadSlot(int _slotIndex)
        {
            if (!isValidSlotIndex(_slotIndex))
                return null;

            string path = GetSlotFilePath(_slotIndex);
            if (!File.Exists(path))
                return null;

            try
            {
                string json = File.ReadAllText(path, Encoding.UTF8);
                StorySaveSlotData data = JsonUtility.FromJson<StorySaveSlotData>(json);
                if (data == null || !data.HasData())
                    return null;
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[StorySaveMgr] 读取槽位 {_slotIndex} 失败：{e.Message}");
                return null;
            }
        }

        public static bool HasSlotData(int _slotIndex)
        {
            return ReadSlot(_slotIndex) != null;
        }

        public static bool SaveCurrentToSlot(int _slotIndex)
        {
            if (!isValidSlotIndex(_slotIndex))
                return false;

            StoryModel model = StoryModel.instance;
            if (model.currentChapterId <= 0 || model.currentNodeId <= 0)
            {
                Debug.LogWarning("[StorySaveMgr] 当前无有效剧情进度，无法存档");
                return false;
            }

            ChapterRefObj chapter = StoryRefDataHelper.GetChapter(model.currentChapterId);
            DateTime now = DateTime.Now;
            StorySaveSlotData data = new StorySaveSlotData
            {
                saveVersion = StorySaveSlotData.CurrentSaveVersion,
                chapterId = model.currentChapterId,
                nodeId = model.currentNodeId,
                clearedChapterIds = model.clearedChapterIds?.ToArray() ?? Array.Empty<int>(),
                chapterName = chapter != null ? chapter.chapterTitle : string.Empty,
                saveTimeTicks = now.Ticks,
                saveTimeText = now.ToString("yyyy-MM-dd HH:mm"),
            };

            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(GetSlotFilePath(_slotIndex), json, Encoding.UTF8);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[StorySaveMgr] 写入槽位 {_slotIndex} 失败：{e.Message}");
                return false;
            }
        }

        public static bool LoadSlotToModel(int _slotIndex)
        {
            StorySaveSlotData data = ReadSlot(_slotIndex);
            if (data == null)
                return false;

            StoryNodeRefObj node = StoryRefDataHelper.GetNode(data.chapterId, data.nodeId);
            if (node == null)
            {
                Debug.LogError($"[StorySaveMgr] 槽位 {_slotIndex} 节点无效：chapter={data.chapterId}, node={data.nodeId}");
                return false;
            }

            StoryModel.instance.RestoreState(data.chapterId, data.nodeId, data.clearedChapterIds);
            SCMsgCenter.SendMsg(SCMsgConst.STORY_SAVE_LOADED, data.chapterId, data.nodeId);
            return true;
        }

        private static bool isValidSlotIndex(int _slotIndex)
        {
            return _slotIndex >= 0 && _slotIndex < MaxSlotCount;
        }
    }
}
