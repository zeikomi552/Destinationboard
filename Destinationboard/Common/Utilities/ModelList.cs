using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destinationboard.Common.Utilities
{
    public class ModelList<T>
    {
		#region リスト要素[Items]プロパティ
		/// <summary>
		/// リスト要素[Items]プロパティ用変数
		/// </summary>
		ObservableCollection<T> _Items = new ObservableCollection<T>();
		/// <summary>
		/// リスト要素[Items]プロパティ
		/// </summary>
		public ObservableCollection<T> Items
		{
			get
			{
				return _Items;
			}
			set
			{
				if (_Items == null || !_Items.Equals(value))
				{
					_Items = value;
					NotifyPropertyChanged("Items");
				}
			}
		}
		#endregion

		#region 選択要素[SelectedItem]プロパティ
		/// <summary>
		/// 選択要素[SelectedItem]プロパティ用変数
		/// </summary>
		T _SelectedItem;
		/// <summary>
		/// 選択要素[SelectedItem]プロパティ
		/// </summary>
		public T SelectedItem
		{
			get
			{
				return _SelectedItem;
			}
			set
			{
				if (_SelectedItem == null || !_SelectedItem.Equals(value))
				{
					_SelectedItem = value;
					NotifyPropertyChanged("SelectedItem");
				}
			}
		}
		#endregion

		#region 上へ移動処理
		/// <summary>
		/// 上へ移動処理
		/// </summary>
		public void MoveUp()
		{
			if (this.SelectedItem != null)
			{
				int index = this.Items.IndexOf(this.SelectedItem);

				if (index > 0)
				{
					// 指定した位置の要素を取り出す
					var elem = this.Items.ElementAt(index);
					// 指定した位置の要素を削除する
					this.Items.RemoveAt(index);
					// 一つ上の要素に挿入する
					this.Items.Insert(index - 1, elem);
					// 選択要素をセット
					this.SelectedItem = elem;
				}
			}
		}
		#endregion

		#region 下へ移動処理
		/// <summary>
		/// 下へ移動処理
		/// </summary>
		public void MoveDown()
		{
			if (this.SelectedItem != null)
			{
				int index = this.Items.IndexOf(this.SelectedItem);

				if (index < this.Items.Count - 1)
				{
					// 指定した位置の要素を取り出す
					var elem = this.Items.ElementAt(index);
					// 指定した位置の要素を削除する
					this.Items.RemoveAt(index);
					// 一つ上の要素に挿入する
					this.Items.Insert(index + 1, elem);
					// 選択要素をセット
					this.SelectedItem = elem;
				}
			}
		}
		#endregion

		#region 選択要素の削除処理
		/// <summary>
		/// 選択要素の削除処理
		/// </summary>
		public void DeleteSelectedItem()
		{
			// nullチェック
			if (this.SelectedItem != null)
			{
				// 要素の削除
				this.Items.Remove(this.SelectedItem);
			}
		}
		#endregion

		#region インデックスで指定した値を取得する
		/// <summary>
		/// インデックスで指定した値を取得する
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T ElementAt(int index)
		{
			return this.Items.ElementAt(index);
		}
		#endregion

		#region 追加処理
		/// <summary>
		/// 要素の追加
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			this.Items.Add(item);
		}
		#endregion

		#region INotifyPropertyChanged 
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}
		#endregion

	}
}
