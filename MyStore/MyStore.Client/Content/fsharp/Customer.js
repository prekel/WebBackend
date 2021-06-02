import { interpolate, toText } from "./.fable/fable-library.3.1.11/String.js";
import { Customer_ToDomain_Z54B26620 } from "./MyStore.Domain/Shop.js";
import { createElement } from "react";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";
import { Interop_reactApi } from "./.fable/Feliz.1.45.0/Interop.fs.js";

export function Customer(customerModel) {
    let value;
    const children = singleton((value = toText(interpolate("%A%P()", [Customer_ToDomain_Z54B26620(customerModel)])), createElement("p", {
        children: [value],
    })));
    return createElement("div", {
        children: Interop_reactApi.Children.toArray(Array.from(children)),
    });
}

