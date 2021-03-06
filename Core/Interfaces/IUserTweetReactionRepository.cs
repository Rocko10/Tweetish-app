using TweetishApp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TweetishApp.Core.Interfaces
{
    public interface IUserTweetReactionRepository
    {
        Task<UserTweetReaction> Toggle(UserTweetReaction userTweetReaction);
        Task<UserTweetReaction> Reacted(UserTweetReaction userTweetReaction);
        Task<IEnumerable<UserTweetReaction>> ReactedToMany(IEnumerable<UserTweetReaction> reactions);
    }
}