using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace IUV.SDN
{
    public class MenuView
    {
        public GComponent View;
        GGraph bg;
        GGroup menu;

        Dictionary<string, EventCallback1> calls = new Dictionary<string, EventCallback1>();

        Vector3 mOldPos;
        public MenuView(GComponent view)
        {
            View = view;
            bg = view.GetChild("bg").asGraph;
            menu = view.GetChild("menu").asGroup;
            bg.onClick.Add(OnBgClick);
            bg.onRightClick.Add(OnBgClick);
            calls.Clear();
        }

        void OnTouchBegin(EventContext c)
        {
            c.StopPropagation();
        }

        public void SetMenuClick(string menuItem, EventCallback1 c)
        {
            EventCallback1 call;
            if (calls.TryGetValue(menuItem, out call))
            {
                calls[menuItem] = c;
            }
            else
            {
                calls.Add(menuItem, c);
            }
            var item = View.GetChildInGroup(menu, menuItem);
            item.onClick.Add(OnItemClick);
        }

        public void SetMenuView(string menuItem, bool visible)
        {
            var item = View.GetChildInGroup(menu, menuItem);
            item.visible = visible;
        }

        void OnItemClick(EventContext c)
        {
            var sender = c.sender as GComponent;
            var menuItem = sender.name;

            EventCallback1 call;
            if (calls.TryGetValue(menuItem, out call))
            {
                call.Invoke(c);
            }

            View.visible = false;
        }

        public void SetPosition(Vector3 gPosition, Vector2 size, bool move = true)
        {
            mOldPos = View.GlobalToLocal(gPosition) + (move ? new Vector2(100, 0) : Vector2.zero);
            ShowPos(mOldPos, size, move);
        }

        void ShowPos(Vector3 pos, Vector2 size, bool move)
        {
            var r = size.x - pos.x;
            var b = size.y - pos.y;
            menu.position = mOldPos;
            if (r < menu.width)
            {
                menu.position = menu.position - new Vector3(menu.width, 0, 0) - (move ? new Vector3(100, 0, 0) : Vector3.zero);
            }
            if (b < menu.height)
            {
                menu.position = menu.position - new Vector3(0, menu.height, 0) - (move ? new Vector3(0, b, 0) : Vector3.zero);
            }
        }

        void OnBgClick()
        {
            View.visible = false;
        }
    }
}