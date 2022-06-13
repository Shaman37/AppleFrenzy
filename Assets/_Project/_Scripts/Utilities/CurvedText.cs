using UnityEngine;
using System.Collections;
using TMPro;

[ExecuteInEditMode]
public class CurvedText : MonoBehaviour
{
        private TMP_Text tmpTextComponent;

        public AnimationCurve vertexCurve;
        public float angleMultiplier;
        public float speedMultiplier;
        public float curveScale;

        private void Awake()
        {
            tmpTextComponent = gameObject.GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            StartCoroutine(WarpText());
        }

        private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
        {
            AnimationCurve newCurve = new AnimationCurve();

            newCurve.keys = curve.keys;

            return newCurve;
        }

        /// <summary>
        ///     Method to curve text along a Unity animation curve.
        /// </summary>
        /// <returns></returns>
        IEnumerator WarpText()
        {
            vertexCurve.preWrapMode = WrapMode.Clamp;
            vertexCurve.postWrapMode = WrapMode.Clamp;

            Vector3[] vertices;
            Matrix4x4 matrix;

            tmpTextComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
            float old_CurveScale = curveScale;
            AnimationCurve old_curve = CopyAnimationCurve(vertexCurve);

            while (true)
            {
                if (!tmpTextComponent.havePropertiesChanged && old_CurveScale == curveScale && old_curve.keys[1].value == vertexCurve.keys[1].value)
                {
                    yield return null;
                    continue;
                }

                old_CurveScale = curveScale;
                old_curve = CopyAnimationCurve(vertexCurve);

                tmpTextComponent.ForceMeshUpdate(); // Generate the mesh and populate the textInfo with data we can use and manipulate.

                TMP_TextInfo textInfo = tmpTextComponent.textInfo;
                int characterCount = textInfo.characterCount;


                if (characterCount == 0) continue;

                float boundsMinX = tmpTextComponent.bounds.min.x;
                float boundsMaxX = tmpTextComponent.bounds.max.x;

                for (int i = 0; i < characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible)
                        continue;

                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the index of the mesh used by this character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    vertices = textInfo.meshInfo[materialIndex].vertices;

                    // Compute the baseline mid point for each character
                    Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, textInfo.characterInfo[i].baseLine);

                    // Apply offset to adjust our pivot point.
                    vertices[vertexIndex + 0] += -offsetToMidBaseline;
                    vertices[vertexIndex + 1] += -offsetToMidBaseline;
                    vertices[vertexIndex + 2] += -offsetToMidBaseline;
                    vertices[vertexIndex + 3] += -offsetToMidBaseline;

                    // Compute the angle of rotation for each character based on the animation curve
                    float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX); // Character's position relative to the bounds of the mesh.
                    float x1 = x0 + 0.0001f;
                    float y0 = vertexCurve.Evaluate(x0) * curveScale;
                    float y1 = vertexCurve.Evaluate(x1) * curveScale;

                    Vector3 horizontal = new Vector3(1, 0, 0);
                    Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

                    float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                    Vector3 cross = Vector3.Cross(horizontal, tangent);
                    float angle = cross.z > 0 ? dot : 360 - dot;

                    matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                    vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
                    vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                    vertices[vertexIndex + 0] += offsetToMidBaseline;
                    vertices[vertexIndex + 1] += offsetToMidBaseline;
                    vertices[vertexIndex + 2] += offsetToMidBaseline;
                    vertices[vertexIndex + 3] += offsetToMidBaseline;
                }

                // Upload the mesh with the revised information
                tmpTextComponent.UpdateVertexData();

                yield return new WaitForSeconds(0.025f);
        }
    }
}