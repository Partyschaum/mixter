module Mixter.Domain.Core.Subscription

open Mixter.Domain.Identity.UserIdentity

type SubscriptionId = { Follower: UserId; Followee: UserId }

type Event = 
    | UserFollowed of UserFollowed
and UserFollowed = { SubscriptionId: SubscriptionId }

let follow follower followee =
    [ UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } } ]


