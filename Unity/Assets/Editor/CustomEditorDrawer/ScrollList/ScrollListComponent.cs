using System;
using System.Collections;
using UnityEngine;

namespace ETEditor
{
	public class ScrollListComponent : LiteEntity
	{
		public IList list;

		public Rect TitleRect;

		public string Title;
		
		public Vector2 ScrollPosition;

		public int SelectedItemIndex = -1;

		public bool ShowSelectBox = false;

		public Rect ContentRect;
		
		public int RowHeight;

		public int RowWidth;
		
		public Action<ScrollListComponent,int> OnScrollItemLeftClick;

		public Action<ScrollListComponent,int> OnScrollItemRightClick;

		public Action<ScrollListComponent> OnAddItemBtnClick;

		public Action<ScrollListComponent> OnSaveBtnClick;

		public Action<ScrollListComponent> OnRefreshBtnClick;

		public GUIStyle GreyBorderStyle;

		public GUIStyle GreenBorderStyle;

		public Texture2D BorderTexture;

		public Texture AddIcon;

		public Texture RemoveIcon;

		public Texture SaveIcon;

		public Texture RefreshIcon;

	}
}

