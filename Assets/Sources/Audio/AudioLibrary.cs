using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2017.CrossContext.Services
{
    public class AudioLibrary : MonoBehaviour
    {
        [Serializable]
        public class Item
        {
            public AudioId id;
            public AudioClip clip;
            public float volume = 1f;
        }

        public List<Item> items;
    }
}