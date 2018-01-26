module Mixter.Infrastructure.Core

open Mixter.Domain.Core.Timeline

open System.Collections.Generic

type MemoryTimelineMessageStore() =
    let store = new HashSet<TimelineMessage>()

    member x.Save timelineMessage =
        store.Add timelineMessage |> ignore

    member x.GetMessagesOfUser userId =
        store |> Seq.filter (fun p -> p.Owner = userId)

    member x.Delete messageId =
        store.RemoveWhere(fun p -> p.MessageId = messageId) |> ignore
