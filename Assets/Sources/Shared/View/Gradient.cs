using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class Gradient : BaseMeshEffect
{
	public Color32 topColor = Color.white;
	public Color32 bottomColor = Color.black;

	public bool leftToRight;

	public override void ModifyMesh(VertexHelper helper)
	{
		if (!IsActive() || helper.currentVertCount == 0)
			return;

		List<UIVertex> vertices = new List<UIVertex>();
		helper.GetUIVertexStream(vertices);

		float bottomY = vertices[0].position.y;
		float topY = vertices[0].position.y;
		float leftX = vertices[0].position.x;
		float rightX = vertices[0].position.x;

		for (int i = 1; i < vertices.Count; i++)
		{
			float y = vertices[i].position.y;
			if (y > topY)
			{
				topY = y;
			}
			else if (y < bottomY)
			{
				bottomY = y;
			}

			float x = vertices[i].position.x;
			if (x > rightX)
			{
				rightX = x;
			}
			else if (x < leftX)
			{
				leftX = x;
			}
		}

		float uiElementHeight = topY - bottomY;
		float uiElementWidth = rightX - leftX;

		UIVertex v = new UIVertex();

		for (int i = 0; i < helper.currentVertCount; i++)
		{
			helper.PopulateUIVertex(ref v, i);

			v.color = leftToRight
				? Color32.Lerp(bottomColor, topColor, (v.position.x - leftX) / uiElementWidth)
				: Color32.Lerp(bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);

			helper.SetUIVertex(v, i);
		}
	}
}