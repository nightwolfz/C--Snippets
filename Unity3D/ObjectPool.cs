﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Plugins
{
    // Repository of commonly used prefabs. 
    [AddComponentMenu("Gameplay/ObjectPool")]

    public class ObjectPool : MonoBehaviour 
    {
        public static ObjectPool Instance { get; private set; }

        #region member

        /// <summary>
        /// Member class for a prefab entered into the object pool
        /// </summary>
        [Serializable]
        public class ObjectPoolEntry 
        {
            /// <summary>
            /// the object to pre instantiate
            /// </summary>
            [SerializeField]
            public GameObject Prefab;

            /// <summary>
            /// quantity of object to pre-instantiate
            /// </summary>
            [SerializeField]
            public int Count;
        }
        #endregion

        /// <summary>
        /// The object prefabs which the pool can handle
        /// by The amount of objects of each type to buffer.
        /// </summary>
        public ObjectPoolEntry[] Entries;


        /// <summary>
        /// The pooled objects currently available.
        /// Indexed by the index of the objectPrefabs
        /// </summary>
        [HideInInspector]
        public List<GameObject>[] Pool;

        /// <summary>
        /// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
        /// </summary>
        protected GameObject ContainerObject;

        void OnEnable()
        {
            Instance = this;
        }


        void Start()
        {
            ContainerObject = new GameObject("ObjectPool");

            //Loop through the object prefabs and make a new list for each one.
            //We do this because the pool can only support prefabs set to it in the editor,
            //so we can assume the lists of pooled objects are in the same order as object prefabs in the array
            
            //Pool = new List<GameObject>[Pool.Length];
            Pool = new List<GameObject>[Entries.Length]; //nwz

            for (int i = 0; i < Entries.Length; i++)
            {
                var objectPrefab = Entries[i];

                //create the repository
                Pool[i] = new List<GameObject>();  

                //fill it
                for (int n = 0; n < objectPrefab.Count; n++)
                {
                    var newObj = Instantiate(objectPrefab.Prefab) as GameObject;

                    if (newObj != null)
                    {
                        newObj.name = objectPrefab.Prefab.name;
                        PoolObject(newObj);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
        /// then null will be returned.
        /// </summary>
        /// <returns>
        /// The object for type.
        /// </returns>
        /// <param name='objectType'>
        /// Object type.
        /// </param>
        /// <param name='onlyPooled'>
        /// If true, it will only return an object if there is one currently pooled.
        /// </param>
        /// <param name='startPosition'>
        /// Set the spawn position
        /// </param>

        public GameObject GetObjectForType(string objectType, bool onlyPooled, Vector3 startPosition)
        {
            for (int i = 0; i < Entries.Length; i++)
            {
                var prefab = Entries[i].Prefab;

                if (prefab.name != objectType) continue;

                if (Pool[i].Count > 0)
                {
                    GameObject pooledObject = Pool[i][0];

                    Pool[i].RemoveAt(0);

                    pooledObject.transform.parent = null;
                    pooledObject.transform.position = startPosition;
                    pooledObject.SetActive(true);

                    return pooledObject;
                }

                if (!onlyPooled) return Instantiate(Entries[i].Prefab, startPosition, Quaternion.identity) as GameObject;
            }

            //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
            return null;
        }

        /// <summary>
        /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
        /// </summary>
        /// <param name='obj'>
        /// Object to be pooled.
        /// </param>

        public void PoolObject(GameObject obj)

        {
            for (int i = 0; i < Entries.Length; i++)
            {
                //Debug.Log("Testing " + obj.name + " <> " + Entries[i].Prefab.name);

                if (Entries[i].Prefab.name != obj.name) continue;

                obj.SetActive(false);
                obj.transform.parent = ContainerObject.transform;

                Pool[i].Add(obj);

                return;
            }
        }
    }
}
