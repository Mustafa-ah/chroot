using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Scanner.ViewHolder
{
    public class CargedAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView Provider { get; private set; }
        public TextView Number { get; private set; }
        public TextView Date { get; private set; }

        public CargedAdapterViewHolder(View itemView) : base(itemView)
        {
            Provider = itemView.FindViewById<TextView>(Resource.Id.textViewProvider);
            Number = itemView.FindViewById<TextView>(Resource.Id.textViewnumber);
            Date = itemView.FindViewById<TextView>(Resource.Id.textViewDate);
        }
    }
}