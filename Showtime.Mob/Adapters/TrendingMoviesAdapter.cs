using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Content.Res;
using AndroidX.RecyclerView.Widget;
using Com.Bumptech.Glide;
using Showtime.Mob.Models;

namespace Showtime.Mob.Adapters
{
    public class TrendingMoviesAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        public List<TrendingMovieDetails> _trendingMovies;

        public class TrendingViewHolder : RecyclerView.ViewHolder
        {
            internal ImageView poster;
            internal TextView title;

            public TrendingViewHolder(View view) : base(view)
            {
                poster = view.FindViewById<ImageView>(Resource.Id.trending_movie_card_poster);
                title = view.FindViewById<TextView>(Resource.Id.trending_movie_card_text);
            }
        }

        public TrendingMoviesAdapter(Context context, List<TrendingMovieDetails> trendingMovies)
        {
            _context = context;
            _trendingMovies = trendingMovies;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.trending_movie_card, parent, false);
            return new TrendingViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as TrendingViewHolder;
            var posterUrl = $"https://image.tmdb.org/t/p/w500{_trendingMovies[position].ImageUrl}";
            var placeholder = AppCompatResources.GetDrawable(_context, Resource.Drawable.placeholder_large_movie);
            Glide.With(_context).Load(posterUrl).Into(vh.poster).OnLoadStarted(placeholder);
            vh.title.Text = _trendingMovies[position].Title;
        }

        public override int ItemCount => _trendingMovies.Count;
    }
}
