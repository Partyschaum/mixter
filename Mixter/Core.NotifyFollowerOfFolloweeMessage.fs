module Mixter.Domain.Core.NotifyFollowerOfFolloweeMessage

open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription
open Mixter.Domain.Core.Message

let notifyAllFollowers (getSubscriptionHistory: SubscriptionId -> Subscription.Event list) (getFollowers: UserId -> UserId seq) (followee: UserId) messageId =
    getFollowers followee 
    |> Seq.collect (fun follower -> 
        { Follower = follower; Followee = followee }
        |> getSubscriptionHistory 
        |> Subscription.apply
        |> Subscription.notifyFollower messageId)

let handle getSubscriptionHistory getFollowers evt =
    let notifyAllFollowers = notifyAllFollowers getSubscriptionHistory getFollowers

    match evt with
    | MessageQuacked e -> notifyAllFollowers e.AuthorId e.MessageId
    | MessageRequacked e -> notifyAllFollowers e.Requacker e.MessageId
    | _ -> Seq.empty
