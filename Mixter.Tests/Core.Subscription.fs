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
        let follower = UserId "follower@mix-it.fr"
        let followee = UserId "followee@mix-it.fr"

        follow follower followee
            |> should equal [UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } }]

    [<Test>] 
    member x.``When unfollow Then UserUnfollowed is returned`` () =
        let subscription = { Follower = UserId "follower@mix-it.fr"; Followee = UserId "followee@mix-it.fr" }

        [UserFollowed { SubscriptionId = subscription }]
            |> apply
            |> unfollow
            |> should equal [UserUnfollowed { SubscriptionId = subscription }]

    [<Test>] 
    member x.``When notify follower Then FolloweeMessageQuacked`` () =
        let subscription = { Follower = UserId "follower@mix-it.fr"; Followee = UserId "followee@mix-it.fr" }
        let message = MessageId.generate()

        [UserFollowed { SubscriptionId = subscription}]
            |> apply
            |> notifyFollower message
            |> should equal [FolloweeMessageQuacked { SubscriptionId = subscription; Message = message}]