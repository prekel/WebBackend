CREATE TABLE public.customer (
    customerid SERIAL NOT NULL,
    firstname varchar(60) NOT NULL,
    lastname varchar(60),
    honorific varchar(30),
    currentcartid integer,
    email varchar(60) NOT NULL,
    passwordhash bytea NOT NULL,
    passwordsalt integer NOT NULL,
    PRIMARY KEY (customerid)
);

CREATE INDEX ON public.customer
    (currentcartid);


CREATE TABLE public.porder (
    orderid SERIAL NOT NULL,
    customerid integer NOT NULL,
    PRIMARY KEY (orderid)
);

CREATE INDEX ON public.porder
    (customerid);


CREATE TABLE public.product (
    productid SERIAL NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    cost numeric NOT NULL,
    PRIMARY KEY (productid)
);


CREATE TABLE public.supportoperator (
    supportoperatorid SERIAL NOT NULL,
    firstname varchar(60) NOT NULL,
    lastname varchar(60) NOT NULL,
    email varchar(60) NOT NULL,
    passwordhash bytea NOT NULL,
    passwordsalt integer NOT NULL,
    PRIMARY KEY (supportoperatorid)
);


CREATE TABLE public.cart (
    cartid SERIAL NOT NULL,
    ownercustomerid integer,
    PRIMARY KEY (cartid)
);

CREATE INDEX ON public.cart
    (ownercustomerid);


CREATE TABLE public.orderorderedproduct (
    orderid integer NOT NULL,
    orderedproductid integer NOT NULL
);

CREATE INDEX ON public.orderorderedproduct
    (orderid);
CREATE INDEX ON public.orderorderedproduct
    (orderedproductid);


CREATE TABLE public.cartproduct (
    cartid integer NOT NULL,
    productid integer NOT NULL
);

CREATE INDEX ON public.cartproduct
    (cartid);
CREATE INDEX ON public.cartproduct
    (productid);


CREATE TABLE public.supportticket (
    supportticketid SERIAL NOT NULL,
    customerid integer NOT NULL,
    supportoperatorid integer NOT NULL,
    PRIMARY KEY (supportticketid)
);

CREATE INDEX ON public.supportticket
    (customerid);
CREATE INDEX ON public.supportticket
    (supportoperatorid);


CREATE TABLE public.supportquestion (
    supportquestionid SERIAL NOT NULL,
    supportticketid integer NOT NULL,
    sendtimestamp timestamp without time zone NOT NULL,
    readtimestamp timestamp without time zone,
    text text NOT NULL,
    PRIMARY KEY (supportquestionid)
);

CREATE INDEX ON public.supportquestion
    (supportticketid);


CREATE TABLE public.supportanswer (
    supportanswerid SERIAL NOT NULL,
    supportticketid integer NOT NULL,
    supportoperatorid integer NOT NULL,
    sendtimestamp timestamp without time zone NOT NULL,
    text text NOT NULL,
    PRIMARY KEY (supportanswerid)
);

CREATE INDEX ON public.supportanswer
    (supportticketid);
CREATE INDEX ON public.supportanswer
    (supportoperatorid);


CREATE TABLE public.orderedproduct (
    productid integer NOT NULL,
    orderedcost numeric NOT NULL,
    PRIMARY KEY (productid)
);


ALTER TABLE public.customer ADD CONSTRAINT FK_customer__currentcartid FOREIGN KEY (currentcartid) REFERENCES public.cart(cartid);
ALTER TABLE public.porder ADD CONSTRAINT FK_porder__customerid FOREIGN KEY (customerid) REFERENCES public.customer(customerid);
ALTER TABLE public.cart ADD CONSTRAINT FK_cart__ownercustomerid FOREIGN KEY (ownercustomerid) REFERENCES public.customer(customerid);
ALTER TABLE public.orderorderedproduct ADD CONSTRAINT FK_orderorderedproduct__orderid FOREIGN KEY (orderid) REFERENCES public.porder(orderid);
ALTER TABLE public.orderorderedproduct ADD CONSTRAINT FK_orderorderedproduct__orderedproductid FOREIGN KEY (orderedproductid) REFERENCES public.orderedproduct(productid);
ALTER TABLE public.cartproduct ADD CONSTRAINT FK_cartproduct__cartid FOREIGN KEY (cartid) REFERENCES public.cart(cartid);
ALTER TABLE public.cartproduct ADD CONSTRAINT FK_cartproduct__productid FOREIGN KEY (productid) REFERENCES public.product(productid);
ALTER TABLE public.supportticket ADD CONSTRAINT FK_supportticket__customerid FOREIGN KEY (customerid) REFERENCES public.customer(customerid);
ALTER TABLE public.supportticket ADD CONSTRAINT FK_supportticket__supportoperatorid FOREIGN KEY (supportoperatorid) REFERENCES public.supportoperator(supportoperatorid);
ALTER TABLE public.supportquestion ADD CONSTRAINT FK_supportquestion__supportticketid FOREIGN KEY (supportticketid) REFERENCES public.supportticket(supportticketid);
ALTER TABLE public.supportanswer ADD CONSTRAINT FK_supportanswer__supportticketid FOREIGN KEY (supportticketid) REFERENCES public.supportticket(supportticketid);
ALTER TABLE public.supportanswer ADD CONSTRAINT FK_supportanswer__supportoperatorid FOREIGN KEY (supportoperatorid) REFERENCES public.supportoperator(supportoperatorid);
ALTER TABLE public.orderedproduct ADD CONSTRAINT FK_orderedproduct__productid FOREIGN KEY (productid) REFERENCES public.product(productid);