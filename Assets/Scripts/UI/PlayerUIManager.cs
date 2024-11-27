using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIManager : Singleton<PlayerUIManager>
{
    [SerializeField] public String moveText, interactText;
    [SerializeField] public TMP_Text currentText;

    private void Start()
    {
        currentText.enabled = false;
    }

    private void Update()
    {
        if (currentText != null)
        {
            currentText.ForceMeshUpdate();
            var textInfo = currentText.textInfo;

            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                {
                    continue;
                }

                var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

                for (int j = 0; j < 4; ++j)
                {
                    var orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] =
                        orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; ++i)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                currentText.UpdateGeometry(meshInfo.mesh, i);
            }

            if (PlayerController.Instance.movement != Vector2.zero)
            {
                currentText.enabled = false;
            }
        }
    }

    public void ShowText()
    {
        currentText.enabled = true;
    }
}
