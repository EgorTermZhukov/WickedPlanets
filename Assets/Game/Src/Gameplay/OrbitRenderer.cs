using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Src.Gameplay
{
    public class OrbitRenderer : MonoBehaviour
    {
        [FormerlySerializedAs("LineRenderer")] public LineRenderer CircleRenderer;
        public LineRenderer NotchPfb;
        public List<LineRenderer> Notches;

        public void DrawCircle(int steps, float radius)
        {
            CircleRenderer.positionCount = steps;
            for (int currentStep = 0; currentStep < steps; currentStep++)
            {
                // take this into the orbit too
                float circumferenceProgress = (float)currentStep / steps;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                
                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float x = xScaled * radius;
                float y = yScaled * radius;

                Vector3 currentPosition = new Vector3(x, y, 0f);
                
                CircleRenderer.SetPosition(currentStep, currentPosition);
            }
        }

        public void DrawNotches(int notches, float radius, float notchHalfSize)
        {
            for (int currentNotch = 0; currentNotch < notches; currentNotch++)
            {
                var lineRenderer = Instantiate(NotchPfb);
                float circumferenceProgress = (float)currentNotch / notches;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;
                
                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);

                float centerX = xScaled * radius;
                float centerY = yScaled * radius;

                float firstPosX = centerX - xScaled * notchHalfSize;
                float secondPosX = centerX + xScaled * notchHalfSize;

                float firstPosY = centerY - yScaled * notchHalfSize;
                float secondPosY = centerY + yScaled * notchHalfSize;

                lineRenderer.positionCount = 2;
                
                lineRenderer.SetPosition(0, new Vector3(firstPosX, firstPosY, 0f));
                lineRenderer.SetPosition(1, new Vector3(secondPosX, secondPosY, 0f));

                lineRenderer.transform.SetParent(transform);
                Notches.Add(lineRenderer);
            }
        }
    }
}