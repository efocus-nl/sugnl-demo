using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoC.Persistence;
using BoC.UnitOfWork;
using Sitecore.SecurityModel;
using TweetSharp;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<TwitterVisitor> _repository;
        private TwitterService _twitterService;

        public HomeController(IRepository<TwitterVisitor> repository)
        {
            _repository = repository;
            _twitterService = new TwitterService("YOUR_TWITTER_API_KEY", "YOUR_TWITTER_SECRET");
        }

        public ActionResult SendTweet(TweetMessage dataSource, string oauth_token, string oauth_verifier)
        {
            var model = new SendViewModel();
            model.Messages = new List<string>();
            model.TwitterUrl = GetAuthUrl();
            if (!string.IsNullOrEmpty(oauth_token))
            {
                var user = GetTwitterUser(oauth_token, oauth_verifier);
                if (user != null)
                {
                    using (new SecurityDisabler())
                    using (UnitOfWork.BeginUnitOfWork())
                    {
                        var users = _repository.Query().OrderBy(a => a.Name).ToList();
                        foreach (var account in users)
                        {
                            var msg = String.Format(dataSource.TweetText, account.Name);
                            _twitterService.SendTweet(new SendTweetOptions() {Status = msg});
                            model.Messages.Add(msg);
                        }
                    }
                }
            }

            return View(model);

        }

        // GET: Home
        public ActionResult Index(Homepage contextItem, string oauth_token, string oauth_verifier)
        {
            if (!string.IsNullOrEmpty(oauth_token))
            {
                var user = GetTwitterUser(oauth_token, oauth_verifier);
                if (user != null)
                {
                    using (new SecurityDisabler())
                    using (UnitOfWork.BeginUnitOfWork())
                    {
                        var existing =
                            from account in _repository.Query()
                            where account.Name == user.ScreenName
                            select account;

                        if (!existing.Any())
                        {
                            _repository.SaveOrUpdate(new TwitterVisitor() { Name = user.ScreenName, Parent = contextItem });
                            ViewBag.Message = user.ScreenName + " added to the sitecore repository";
                        }
                        else
                        {
                            ViewBag.Message = user.ScreenName + " was already in the sitecore repository";
                        }
                    }
                }
            }
            ViewBag.TwitterUrl = GetAuthUrl();
            return View(contextItem);
        }

        private Uri GetAuthUrl()
        {
            return _twitterService.GetAuthenticationUrl(_twitterService.GetRequestToken(Request.Url.ToString()));
        }

        private TwitterUser GetTwitterUser(string oauthToken, string oauthVerifier)
        {
            if (string.IsNullOrEmpty(oauthToken))
                return null;

            var requestToken = new OAuthRequestToken { Token = oauthToken };

            var accessToken = _twitterService.GetAccessToken(requestToken, oauthVerifier);

            _twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            return _twitterService.VerifyCredentials(new VerifyCredentialsOptions());
        }
    }

    public class SendViewModel
    {
        public List<string> Messages { get; set; }
        public Uri TwitterUrl { get; set; }
    }
}