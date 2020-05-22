using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;

using Den.Tools;
using Den.Tools.GUI;
using Den.Tools.GUI.Popup;

namespace MapMagic.Nodes.GUI
{
	public class PopupTesterWindow : EditorWindow
	{
		public void OnGUI ()
		{
			if (Event.current.type == EventType.MouseDown  &&  Event.current.button == 1)
			{
				Item menu = new Item("Menu");
				menu.subItems = new List<Item>()
				{
					new Item("Create", new Item("Map"), new Item("Objects"), new Item("Splines")),
					new Item("Generator"),
					new Item("Group"),
					new Item("Graph")
				};
				menu.color = new Color(0.8f, 0.8f, 0.8f);

				SingleWindow window = new SingleWindow() {rootItem=menu, width=150};
				window.Show(Event.current.mousePosition);
			}
		}

		[MenuItem ("Window/Test/Popup")]
		public static void ShowWindow ()
		{
			PopupTesterWindow window = (PopupTesterWindow)GetWindow(typeof (PopupTesterWindow));
			window.titleContent = new GUIContent("Popup Tester");
			window.position = new Rect(100,100,300,250);
		}

		
	}
}