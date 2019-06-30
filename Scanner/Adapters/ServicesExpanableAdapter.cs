using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Scanner.Models;

namespace Scanner.Adapters
{
    public class ServicesExpanableAdapter : BaseExpandableListAdapter
    {
        Activity activity;
        List<Card> itemsList;
        public ServicesExpanableAdapter(Activity activity_, List<Card> ItemsList)
            : base()
        {
            activity = activity_;
            itemsList = ItemsList;
        }

        public override int GroupCount
        {
            get
            {
                return itemsList.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return (long)itemsList[groupPosition].Services[childPosition].CardId;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return itemsList[groupPosition].Services.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View child = activity.LayoutInflater.Inflate(Resource.Layout.list_item_services_child, null);
            Card card = itemsList[groupPosition].Services[childPosition];
            child.FindViewById<TextView>(Resource.Id.textViewDetails).Text = card.Description;
            return child;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return (long)itemsList[groupPosition].CardId;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View group = activity.LayoutInflater.Inflate(Resource.Layout.list_item_services_parent, null);
            Card card = itemsList[groupPosition];
            group.FindViewById<TextView>(Resource.Id.textViewProvider).Text = card.ProiderName;
            return group;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}