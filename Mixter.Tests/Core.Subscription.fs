namespace Mixter.Tests.Domain.Core.Subscription

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Subscription

type ``Subscription should`` ()=
    [<Fact>] 
    member x.``When follow Then UserFollowed is returned`` () =
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }

        test <@ follow follower followee
                    = [UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } }] @>

    [<Fact>] 
    member x.``When unfollow Then UserUnfollowed is returned`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr" }; Followee = { Email = "followee@mix-it.fr" } }
        let history = [UserFollowed { SubscriptionId = subscription }]

        test 
          <@ history |> apply |> unfollow
                = [UserUnfollowed { SubscriptionId = subscription }] @>

    [<Fact>] 
    member x.``When notify follower Then FolloweeMessageQuacked`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr" }; Followee = { Email = "followee@mix-it.fr"} }
        let message = MessageId.Generate()
        let history = [UserFollowed { SubscriptionId = subscription}]

        test <@ history |> apply |> notifyFollower message
                    = [FolloweeMessageQuacked { SubscriptionId = subscription; Message = message}] @>

    [<Fact>] 
    member x.``Given unfollow When notify follower Then do not returned FollowerMessageQuacked`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr"}; Followee = { Email = "followee@mix-it.fr"} }
        let message = MessageId.Generate()
        let history = [
            UserFollowed { SubscriptionId = subscription}
            UserUnfollowed { SubscriptionId = subscription }
        ]   
        
        test <@ history |> apply |> notifyFollower message |> Seq.isEmpty @>
