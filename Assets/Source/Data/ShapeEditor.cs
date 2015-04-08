#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Shape))]
public class ShapeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		Shape shape = (Shape)target;

		EditorGUILayout.BeginVertical();

		shape.ColorSet = EditorGUILayout.IntField( "Color", shape.ColorSet );

		bool make3x3 = GUILayout.Button( "Set to 3x3" );
		if(make3x3)
		{
			ChangeSize( shape, 3 );
		}

		bool make4x4 = GUILayout.Button( "Set to 4x4" );
		if(make4x4)
		{
			ChangeSize( shape, 4 );
		}

		shape.Rotations = EditorGUILayout.IntSlider( shape.Rotations, 1, 4 );

		for( int i = 0; i < shape.Rotations; ++i )
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "Rotation " + i + ":" );
			DrawRotation( shape, i );
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndVertical();
	}
	private void ChangeSize( Shape shape, int size )
	{
		shape.Size = size;
		for( int i = 0; i < Shape.MAX_ROTATIONS; ++i )
		{
			bool[] newRot = new bool[size*size];
			shape.SetRotation( i, newRot );
		}
	}

	private void DrawRotation( Shape shape, int rotNumber )
	{
		bool[] rotation = shape.GetRotation( rotNumber );

		EditorGUILayout.BeginVertical( EditorStyles.helpBox );
		for(int i = 0; i < shape.Size; ++i)
		{
			EditorGUILayout.BeginHorizontal();
			for(int j = 0; j < shape.Size; ++j)
			{
				int index = shape.Size*i+j;
				rotation[index] = EditorGUILayout.Toggle( rotation[index] );
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.LabelField( " " );
		}
		EditorGUILayout.EndVertical();

		shape.SetRotation( rotNumber, rotation );
	}
}
#endif