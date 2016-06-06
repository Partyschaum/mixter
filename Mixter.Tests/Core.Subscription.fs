namespace Mixter.Tests.Domain.Core.Subscription

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Subscription

[<TestFixture>]
type ``Subscription should`` ()=
    [<Test>] 
    member x.``When follow Then UserFollowed is returned`` () =
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }

        follow follower followee
            |> should equal [UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } }]

    [<Test>] 
    member x.``When unfollow Then UserUnfollowed is returned`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr" }; Followee = { Email = "followee@mix-it.fr" } }

        [UserFollowed { SubscriptionId = subscription }]
            |> apply
            |> unfollow
            |> should equal [UserUnfollowed { SubscriptionId = subscription }]

    [<Test>] 
    member x.``When notify follower Then FolloweeMessageQuacked`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr" }; Followee = { Email = "followee@mix-it.fr"} }
        let message = MessageId.generate()

        [UserFollowed { SubscriptionId = subscription}]
            |> apply
            |> notifyFollower message
            |> should equal [FolloweeMessageQuacked { SubscriptionId = subscription; Message = message}]

    [<Test>] 
    member x.``Given unfollow When notify follower Then do not returned FollowerMessageQuacked`` () =
        let subscription = { Follower = { Email = "follower@mix-it.fr"}; Followee = { Email = "followee@mix-it.fr"} }
        let message = MessageId.generate()

        [
            UserFollowed { SubscriptionId = subscription}
            UserUnfollowed { SubscriptionId = subscription }
        ]   |> apply
            |> notifyFollower message
            |> should equal []