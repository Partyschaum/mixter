namespace Mixter.Tests.Domain.Core.Subscription

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription

[<TestFixture>]
type ``Subscription should`` ()=
    [<Test>] 
    member x.``When follow Then UserFollowed is returned`` () =
        let follower = UserId "follower@mix-it.fr"
        let followee = UserId "followee@mix-it.fr"

        follow follower followee
            |> should equal [UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } }]