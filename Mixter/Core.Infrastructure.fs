module Mixter.Infrastructure.Core

open Mixter.Domain.Core.Timeline

open System.Collections.Generic

type MemoryTimelineMessageStore() =
    let store = new HashSet<TimelineMessage>()

    member this.Save timelineMessage =
        store.Add timelineMessage |> ignore

    member this.GetMessagesOfUser userId =
        store |> Seq.filter (fun p -> p.Owner = userId)

    member this.Delete messageId =
        store.RemoveWhere(fun p -> p.MessageId = messageId) |> ignore
