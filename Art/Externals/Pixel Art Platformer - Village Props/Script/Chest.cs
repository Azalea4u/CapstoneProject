using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        [FoldoutGroup("Reference")]
        public Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened;

        private void Update()
        {
            // Check if the right mouse button is pressed
            if (Input.GetMouseButtonDown(1))  // 1 is for the right mouse button
            {
                if (!IsOpened)
                {
                    IsOpened = true;
                }
                else
                {
                    IsOpened = false;
                }
            }
        }

        [FoldoutGroup("Runtime"),Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            IsOpened = true;
        }

        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }
    }
}
