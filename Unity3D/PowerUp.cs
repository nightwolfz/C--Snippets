﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Behaviors
{
    public class PowerUp : MonoBehaviour
    {
        private GameObject _player;

        void Start () {
            _player = GameObject.Find("Player");
            Destroy(gameObject, 5);
        }
	
        void Update () {
            transform.position += transform.TransformDirection(new Vector2(0, -5)) * Time.deltaTime * 10;
        }

        void AddOrReplaceComponent<T>() where T : Component
        {
            if (_player.GetComponent<T>())
            {
                Destroy(_player.GetComponent<T>());
                _player.AddComponent<T>();
            }
            else _player.AddComponent<T>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player") return;

            // Health power up
            _player.GetComponent<Player>().Health = _player.GetComponent<Player>().MaxHealth;

            // Shields power up
            _player.GetComponent<Player>().Shield = _player.GetComponent<Player>().MaxShield;

            // Temporary weapon speed boost
            AddOrReplaceComponent<PowerUpSpeed>();

            // Temporary weapon dmg boost

            //AddOrReplaceComponent<PowerUpShield>();
            // Temporary immortality

            Destroy(gameObject);
        }

    }
}
