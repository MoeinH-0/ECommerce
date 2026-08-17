const cookieName = "cart-items";

/* -----------------------------
   Ensure Cookie Exists
--------------------------------*/
function ensureCartCookieExists() {
    let raw = $.cookie(cookieName);

    if (!raw || raw === "undefined" || raw === "null") {
        $.cookie(cookieName, JSON.stringify([]), { expires: 2, path: "/" });
        return;
    }

    try {
        let parsed = JSON.parse(raw);
        if (!Array.isArray(parsed)) {
            $.cookie(cookieName, JSON.stringify([]), { expires: 2, path: "/" });
        }
    } catch {
        $.cookie(cookieName, JSON.stringify([]), { expires: 2, path: "/" });
    }
}

/* -----------------------------
   Get Cart Items
--------------------------------*/
function getCartItems() {

    ensureCartCookieExists();

    let raw = $.cookie(cookieName);

    try {
        let items = JSON.parse(raw);
        return Array.isArray(items) ? items : [];
    } catch {
        $.cookie(cookieName, JSON.stringify([]), { expires: 2, path: "/" });
        return [];
    }
}

/* -----------------------------
   Save Cart Items
--------------------------------*/
function saveCartItems(items) {
    $.cookie(cookieName, JSON.stringify(items), { expires: 2, path: "/" });
}

/* -----------------------------
   Add To Cart
--------------------------------*/
function addToCart(id, name, unitPrice, picture, count) {

    let products = getCartItems();

    const existing = products.find(x => x.id === id);

    let parsedCount = parseInt(count);
    if (existing) {
        existing.count += parsedCount;
    } else {
        products.push({
            id: id,
            name: name,
            unitPrice: unitPrice,
            picture: picture,
            count: parsedCount
        });
    }

    saveCartItems(products);

    updateCart();
}

/* -----------------------------
   Remove From Cart
--------------------------------*/
function removeFromCart(id) {

    let products = getCartItems();

    products = products.filter(x => x.id !== id);

    saveCartItems(products);

    updateCart();
}

/* -----------------------------
   Change Item Count
--------------------------------*/
function changeCartItemCount(id, count) {

    let products = getCartItems();

    const item = products.find(x => x.id === id);

    if (!item) return;

    item.count = count;

    if (item.count <= 0) {
        products = products.filter(x => x.id !== id);
    }

    saveCartItems(products);

    updateCart();
}

/* -----------------------------
   Update Cart UI
--------------------------------*/
function updateCart() {

    const products = getCartItems();

    const cartItemsWrapper = $("#cart_items_wrapper");

    if (!cartItemsWrapper.length) return;

    cartItemsWrapper.html('');
    $("#cart_items_count").text(products.length);

    if (products.length === 0) {

        cartItemsWrapper.append(`
            <div class="cart-empty-message" style="padding:20px;text-align:center">
                سبد خرید شما خالی است
            </div>
        `);

        return;
    }

    products.forEach(x => {

        const product = `
        <div class="single-cart-item">
            <a href="javascript:void(0)" class="remove-icon" onclick="removeFromCart('${x.id}')">
                <i class="ion-android-close"></i>
            </a>

            <div class="image">
                <a href="#">
                    <img src="/ProductPictures/${x.picture}" class="img-fluid" alt="">
                </a>
            </div>

            <div class="content">
                <p class="product-title">
                    <a href="#">${x.name}</a>
                </p>

                <p class="count">تعداد: ${x.count}</p>

                <p class="count">قیمت واحد: ${x.unitPrice}</p>
            </div>
        </div>
        `;

        cartItemsWrapper.append(product);
    });
}


/* -----------------------------
   Document Ready
--------------------------------*/
$(document).ready(function () {

    ensureCartCookieExists();

    updateCart();
});
