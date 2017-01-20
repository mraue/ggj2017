using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CreatingDust.GGJ2017.CrossContext.Services
{
    [CustomEditor(typeof(AudioLibrary))]
    public class AudioLibraryEditor : Editor
    {
        List<bool> _foldouts = new List<bool>();

        List<int> _markedForDeletion = new List<int>();

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            AudioLibrary audioLibrary = (AudioLibrary)target;

            while (_foldouts.Count < audioLibrary.items.Count)
            {
                _foldouts.Add(false);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Entry"))
            {
                audioLibrary.items.Add(new AudioLibrary.Item());
                _foldouts.Add(true);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            for (int i = 0; i < audioLibrary.items.Count; i++)
            {
                var item = audioLibrary.items[i];
                _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], string.Format("{0}: {1}/{2}", i, item.id, item.clip != null ? item.clip.name : ""));

                if (_foldouts[i])
                {
                    item.id = (AudioId)EditorGUILayout.EnumPopup("Id", item.id);
                    item.clip = EditorGUILayout.ObjectField("Clip", item.clip, typeof(AudioClip), true) as AudioClip;
                    item.volume = EditorGUILayout.Slider("Volume", item.volume, 0f, 1f);

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Remove"))
                    {
                        _markedForDeletion.Add(i);
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Entry"))
            {
                audioLibrary.items.Add(new AudioLibrary.Item());
                _foldouts.Add(true);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            foreach (var index in _markedForDeletion)
            {
                audioLibrary.items.RemoveAt(index);
                _foldouts.RemoveAt(index);
            }
            _markedForDeletion.Clear();

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}