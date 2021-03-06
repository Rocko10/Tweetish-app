using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using TweetishApp.Core.Interfaces;
using TweetishApp.Core.Entities;

namespace TweetishApp.Core.Services
{
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository _repository;

        public TweetService(ITweetRepository repository)
        {
            _repository = repository;
        }

        public async Task<Tweet> Create(Tweet tweet)
        {
            if (tweet.UserId == null || tweet.Text.Length < 3) {
                throw new ArgumentException("Invalid tweet");
            }

            tweet = await _repository.Create(tweet);

            return tweet;
        }

        public async Task<Tweet> Update(Tweet tweet)
        {
            tweet = await _repository.Update(tweet);

            return tweet;
        }

        public async Task Remove(int id)
        {
            await _repository.Remove(id);
        }

        public async Task<List<Tweet>> GetTweetsBy(string userId)
        {
            return await _repository.GetTweetsBy(userId);
        }
    }
}