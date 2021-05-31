import { some } from "./.fable/fable-library.3.1.11/Option.js";
import { printf, toText } from "./.fable/fable-library.3.1.11/String.js";
import { map, singleton, append, delay, toList } from "./.fable/fable-library.3.1.11/Seq.js";
import { createElement } from "react";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export function Cart(props) {
    console.log(some(props));
    console.log(some(props.cart));
    const s = toText(printf("%A"))(props);
    const children = toList(delay(() => append(singleton(createElement("p", {
        children: [s],
    })), delay(() => map((i) => createElement("p", {
        children: [i.name],
    }), props.cart.products)))));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

