function IndexJs() {

    $('.slick-feedback').slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        dots: false,
        arrows: true,
        prevArrow: '<button type="button" class="previous-feedback"><i class="far fa-long-arrow-alt-left"></i></button>',
        nextArrow: '<button type="button" class="next-feedback"><i class="far fa-long-arrow-alt-right"></i></button>'
        //autoplay: true,
        //autoplayspeed: 6000,
    });


    $(".slick-partner").slick({
        infinite: true,
        arrows: false,
        dots: false,
        slidesToShow: 8,
        slidesToScroll: 8,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                }
            }
        ]
    });


    $('.counter').countUp({
        delay: 10,
        time: 2000
    });
}

$(function () {
    AOS.init();

    $(".bars-mobile").on("click", function () {
        $(this).find("i").toggleClass("fa-bars").toggleClass("fa-times");
        $("body").toggleClass("show-menu");
        if ($(".overlay-all").length) {
            $(".overlay-all").remove();
        }
        else {
            $("body").append("<div class='overlay-all'/>");
        }
    });

    $("#contact_form").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/ContactForm", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Liên hệ thành công',
                        text: data.msg,
                        icon: 'success'
                    })
                    $("#contact_form").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Liên hệ không thành công',
                        text: data.msg,
                        icon: 'error'
                    })
                }
            });
        }
    });

    $("#form-question").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/User/SendQuestion", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Gửi thành công thành công',
                        text: data.msg,
                        icon: 'success'
                    })
                    $("#form-question").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Gửi thất bại, vui lòng thử lại',
                        text: data.msg,
                        icon: 'error'
                    })
                }
            });
        }
    });


    $("#form-comment").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/User/Comment", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Gửi thành công thành công',
                        text: data.msg,
                        icon: 'success',
                    })
                    $("#form-comment").trigger("reset");
                    window.location.reload();
                } else {
                    $.toast({
                        heading: 'Gửi thất bại, vui lòng thử lại',
                        text: data.msg,
                        icon: 'error'
                    })
                }
            });
        }
    });

    $("#contact_form2").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            $.post("/Home/QuickContact", $(this).serialize(), function (data) {
                if (data.status) {
                    $.toast({
                        heading: 'Liên hệ thành công',
                        text: data.msg,
                        icon: 'success'
                    })
                    $("#contact_form2").trigger("reset");
                } else {
                    $.toast({
                        heading: 'Liên hệ không thành công',
                        text: data.msg,
                        icon: 'error'
                    })
                }
            });
        }
    });



    var $inputItem = $(".js-inputWrapper");
    $inputItem.length && $inputItem.each(function () {
        var $this = $(this),
            $input = $this.find(".formRow--input"),
            placeholderTxt = $input.attr("placeholder"),
            $placeholder;

        $input.after('<span class="placeholder">' + placeholderTxt + "</span>"),
            $input.attr("placeholder", ""),
            $placeholder = $this.find(".placeholder"),

            $input.val().length ? $this.addClass("active") : $this.removeClass("active"),

            $input.on("focusout", function () {
                $input.val().length ? $this.addClass("active") : $this.removeClass("active");
            }).on("focus", function () {
                $this.addClass("active");
            });
    });


    $(document).ready(function () {
        $("#list-service").hover(function () {
            $("#sub-menu").css("display", "flex");
        });
    });
    $(document).ready(function () {
        $("#sub-menu").mouseleave(function () {
            $("#sub-menu").css("display", "none");
        });
    });
       
      
    const toggleBtn = document.querySelectorAll('#toggleBtn');
    const divList = document.querySelectorAll('.form-action-comment');

    toggleBtn.addEventListener('click', () => { 
        if (divList.style.display === 'none') {
            divList.style.display = 'flex';
        }
        else {
            divList.style.display = 'none';
        }
    });



});

$("#AlertBox").removeClass('hide');
$("#AlertBox").delay(5000).slideUp(500); 
function ProductDetailJs() {

    $(".product-gallery").slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        asNavFor: '.thumb-nav'
    });
    $(".thumb-nav").slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        asNavFor: '.product-gallery',
        dots: false,
        focusOnSelect: true,
        prevArrow: '<div class="slick-prev"><i class="fal fa-angle-left"></i></div>',
        nextArrow: '<div class="slick-next"><i class="fal fa-angle-right"></i></div>'
    });
    $('[data-fancybox="gallery"]').fancybox({
        protect: true,
        loop: true,
        buttons: [
            'zoom',
            'slideShow',
            'close'
        ],
        animationEffect: "zoom-in-out",
        animationDuration: 366,
        transitionEffect: "tube",
        zoomOpacity: "auto"
    }); 

    $("#orderForm").on("submit", function (e) {
        e.preventDefault();
        $.post("/dat-hang-nhanh", $(this).serialize(), function (data) {
            if (data === "True") {
                $.toast({
                    heading: 'Cảm ơn bạn đã liên hệ. Chúng tôi sẽ liên hệ trong thời gian sớm nhất.',
                    icon: 'success'
                });
                $("#orderForm").trigger("reset");
            } else {
                $.toast({
                    heading: 'Gửi thất bại',
                    text: 'Vui lòng điền đúng định dạng',
                    icon: 'error',
                })
            }
        });
    });
}


//function showElement() {
//    document.getElementById('watchMore').style.display = 'none';
//    document.getElementById('myElement').style.display = 'flex';
//}

//function returnContact() { 
//    window.location.href = '@Url.Action("Contact", "Home")/';
//}


//function confirmAccount() {
//    $.ajax({
//        type: "Post",
//        url: "/gio-hang/them-vao-gio-hang",
//        data: { productId: id },
//        success: function (res) {
//            if (res) {
//                alert("Thêm vào giỏ hàng thành công !!1")
//                window.location.reload();
//            }
//            else {
//                alert("Thêm thất bại");
//            }
//        }
//    });
//}

function PlusItem(id) {
    $.ajax({
        type: "Post",
        url: "/gio-hang/them-vao-gio-hang",
        data: { productId: id },
        success: function (res) {
            if (res) {
                alert("Thêm vào giỏ hàng thành công !!")
                window.location.reload();
            }
            else {
                alert("Thêm thất bại");
            }
        }
    });
}


$(".RemoveLink").click(function () {
    if (confirm("Bạn có chắc chắn xóa sản phẩm này khỏi giỏ hàng?")) {
        const recordToDelete = $(this).attr("data-id");
        if (recordToDelete !== "") {
            $.post("/ShoppingCart/RemoveFromCart", { "id": recordToDelete }, function (data) {
                if (data.Status === "1") {
                    $(`[data-row=${recordToDelete}`).fadeOut("slow");
                    $(".cart-qty").text(data.CartCount);
                    $("#update-message").text(data.Message);
                } else {
                    alert("Quá trình thực hiện không thành công");
                }
            });
        }
    }
});

function deleteProduct(id) {
    if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này?")) {
        $.post("/ShoppingCart/RemoveFromCart", { productId: id }, function (data) {
            if (data) {
                $.toast({
                    text: 'Xóa sản phẩm thành công',
                    position: 'bottom-right',
                    icon: 'success',
                })
                $("tr[data-id='" + id + "']").fadeOut();
            }
            else {
                $.toast({
                    text: 'Quá trình thực hiện không thành công. Hãy thử lại',
                    icon: 'error',
                })
            }
        });
    }
}


const buttons = document.querySelectorAll('a');
buttons.forEach(btn => {
    btn.addEventListener('click', function (e) {
        let x = e.clientX - e.target.offsetLeft;
        let y = e.clientY - e.target.offsetTop;

        let ripples = document.createElement('span');
        ripples.style.left = x + 'px';
        ripples.style.top = y + 'px';
        this.appendChild(ripples);

        setTimeout(() => {
            ripples.remove();
        }, 1000);
    })
})


function addToWishlist(proId, action) {
    $.post("/account/yeu-thich", { proId: proId, action: action }, function (data) {
        alert(data.msg);
        if (data.status === 0) {
            $("[data-item=wishlist]").text(data.count);
            if (action === "remove") {
                $(`[data-wishlist='${proId}']`).remove();
            }
        }
    });
}
function PinComment(commentId, status) {
    $.post("/User/PinComment", { commentId: commentId, status: status }, function (data) {
        if (data) {
            alert("Cập nhật hoàn tất");
            window.location.reload();
        }
        else {
            alert("Quá trình thực hiện không thành công. Hãy thử lại");
        }
    });
}

//href = "@Url.Action("PinComment", new { commentId = ans.Id, status = ""})

function updateCommentStatus(id, status) {
    $.post("/User/UpdateCommentStatus", { postId: id, status }, function (data) {
        if (data) {
            alert("Cập nhật hoàn tất");
            window.location.reload();
        } else {
            alert("Quá trình thực hiện không thành công. Hãy thử lại");
        }
    });
}

