module Mixter.Domain.Core.Subscription

open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

type SubscriptionId = { Follower: UserId; Followee: UserId }

type Event = 
    | UserFollowed of UserFollowed
    | UserUnfollowed of UserUnfollowed
    | FolloweeMessageQuacked of FolloweeMessageQuacked
and UserFollowed = { SubscriptionId: SubscriptionId }
and UserUnfollowed = { SubscriptionId: SubscriptionId }
and FolloweeMessageQuacked = { SubscriptionId: SubscriptionId; Message: MessageId }

type DecisionProjection =
    | NoSubscription
    | Active of SubscriptionId
    | Desactive of SubscriptionId

let follow follower followee =
    [ UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } } ]

let unfollow decisionProjection =
    match decisionProjection with
    | Active subscriptionId -> [ UserUnfollowed { SubscriptionId = subscriptionId } ]
    | _ -> []

let notifyFollower message decisionProjection =
    match decisionProjection with
    | Active subscriptionId -> [ FolloweeMessageQuacked { SubscriptionId = subscriptionId; Message = message } ]
    | _ -> []

let applyOne decisionProjection event =
    match event with
    | UserFollowed e -> Active e.SubscriptionId
    | UserUnfollowed e -> Desactive e.SubscriptionId
    | _ -> decisionProjection

let apply events =
    Seq.fold applyOne NoSubscription events
