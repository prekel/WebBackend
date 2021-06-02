import { map } from "./.fable/fable-library.3.1.11/Array.js";
import { Ticket_ToDomain_5519A0CB } from "./MyStore.Domain/Support.js";
import { map as map_1, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";
import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";

export function Chats(ticketsModel) {
    const tickets = map((arg00) => Ticket_ToDomain_5519A0CB(arg00), ticketsModel.tickets);
    const children = toList(delay(() => map_1((i) => {
        let value;
        return createElement("a", {
            children: Interop_reactApi.Children.toArray([(value = toText(interpolate("%A%P()", [i])), createElement("p", {
                children: [value],
            }))]),
            href: toText(interpolate("/Support/Chat/%i%P()", [i.SupportTicketId])),
        });
    }, tickets)));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

