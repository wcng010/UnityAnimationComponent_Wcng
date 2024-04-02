using System;
using UnityEngine;

namespace Wcng
{
    public static class FunctionLibrary
    {
        public static bool Compare(string str1, string str2)
        {
            return String.Compare(str1, str2, StringComparison.Ordinal) == 0;
        }
        
        public static Vector3 ConvertMoveInputToCameraSpace(Transform cameraTransform, float horz, float vert)
        {
            float moveX = (horz * cameraTransform.right.x) + (vert * cameraTransform.forward.x);
            float moveZ = (horz * cameraTransform.right.z) + (vert * cameraTransform.forward.z);
            return new Vector3(moveX, 0f, moveZ);
        }

    }
}