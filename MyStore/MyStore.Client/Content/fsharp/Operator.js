import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { Operator_ToDomain_Z2DE961B1 } from "./MyStore.Domain/Support.js";
import { createElement } from "react";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export function Operator(operatorModel) {
    let value;
    const children = singleton((value = toText(interpolate("%A%P()", [Operator_ToDomain_Z2DE961B1(operatorModel)])), createElement("p", {
        children: [value],
    })));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

