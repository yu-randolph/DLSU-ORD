function order(docuID, docuName, deliveryRate, packaging, quantity, degree, price){
  this.docuID = docuID;
  this.docuName = docuName;
  this.deliveryRate = deliveryRate;
  this.packaging = packaging;
  this.quantity = quantity;
  this.degree = degree;
  this.price = price;
}
function addCart(docuID, docuName, deliveryRate, packaging, quantity, degree, price){
  var newOrder = new order(docuID, docuName, deliveryRate, packaging, quantity, degree, price);
  if(sessionStorage.getItem("cart") === null){
      var cart = [];
      cart.push(newOrder);
      sessionStorage.cart = JSON.stringify(cart);
      return true;
  }else{
    var cart = JSON.parse(sessionStorage.cart);
    var isNew = true;
    for(var i=0; i<cart.length; i++){
      if(docuID == cart[i].docuID && degree == cart[i].degree){
        isNew = false;
        tempQuantity = parseInt(cart[i].quantity)  + parseInt(quantity);
        if(tempQuantity <=5){
          cart[i].quantity = tempQuantity;
          sessionStorage.cart = JSON.stringify(cart);
          return true;
        }else{
          alert("You cannot order more than 5 of the same document!");
          return false;
        }
      }
    }
    if(isNew){
      cart.push(newOrder);
      sessionStorage.cart = JSON.stringify(cart);
      return true;
    }
  }
}

function removeCart(itemID, degree){
  var cart = JSON.parse(sessionStorage.cart);
  for(var i=0; i<cart.length; i++){
    if(cart[i].docuID == itemID && cart[i].degree == degree){
        cart.splice(i,1);
    }

  }
  sessionStorage.cart = JSON.stringify(cart);
}
function removeCart(itemID, degree){
  var cart = JSON.parse(sessionStorage.cart);
  for(var i=0; i<cart.length; i++){
    if(cart[i].docuID == itemID && cart[i].degree == degree){
        cart.splice(i,1);
    }

  }
  sessionStorage.cart = JSON.stringify(cart);
}

function reloadRemoveCart(itemID, degree){
  var cart = JSON.parse(sessionStorage.cart);
  for(var i=0; i<cart.length; i++){
    if(cart[i].docuID == itemID && cart[i].degree == degree){
        cart.splice(i,1);
    }

  }
  sessionStorage.cart = JSON.stringify(cart);
  populateEditCart();
}

function minusOrder(docuID, degree){
  var cart = JSON.parse(sessionStorage.cart);
  for(var i=0; i<cart.length; i++){
    if(cart[i].docuID == docuID && cart[i].degree == degree){
        if(cart[i].quantity>1){
          cart[i].quantity--;
          sessionStorage.cart = JSON.stringify(cart);
        }else{
          reloadRemoveCart(docuID, degree);
        }
    }
  }
  populateEditCart();
}
function plusOrder(docuID, degree){
  var cart = JSON.parse(sessionStorage.cart);
  for(var i=0; i<cart.length; i++){
    if(cart[i].docuID == docuID && cart[i].degree == degree){
        if(cart[i].quantity<5){
          cart[i].quantity++;
          sessionStorage.cart = JSON.stringify(cart);
        }else{
          alert("You cannot order more than 5 of the same document!")
        }
    }
  }
  populateEditCart();
}

function getProcessing(){
  var cart = JSON.parse(sessionStorage.cart);
  var hasRegular = false;
  var hasExpress = false;
  for(var i=0; i<cart.length; i++){
    if(cart[i].deliveryRate == "regular"){
      hasRegular = true;
    }else if(cart[i].deliveryRate == "express"){
      hasExpress = true;
    }
  }
  if(hasRegular && hasExpress){
    return "Mixed Processing";
  }
  else if(hasExpress){
    return "Express Processing";
  }else{
    return "Regular Processing";
  }
}

function viewCart(){
    $('#viewCart').empty();
    $('#viewCart').append('<table id="cartTable"class="table table-hover"><thead><tr><th>Product</th><th>Quantity</th>'+
    '<th class="text-center">Price</th><th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th></tr></thead></table>');
    if(sessionStorage.cart != null){
    var cart = JSON.parse(sessionStorage.cart);
    for(var i=0; i<cart.length; i++){
        var docuName = cart[i].docuName+"("+cart[i].degree+")";
        var price = cart[i].price * cart[i].quantity;
        $('#cartTable').append('<tr>'+
            '<td>&nbsp;&nbsp;&nbsp;'+docuName+'</td>'+
            '<td>'+cart[i].quantity+'</td>'+
            '<td> Php '+ price +'</td>'+
            '<td>'+
            '    <button type="button" class="btn btn-danger">'+
            '        <span class="glyphicon glyphicon-remove" id="removeCart" onclick="removeCart('+cart[i].docuID+', \''+cart[i].degree+'\');"></span>'+
            '    </button></td></tr>');
    }
  }
}
function getTotalPrice(){
  var cart = JSON.parse(sessionStorage.cart);
  var totalPrice = 0.0;
  for(var i=0; i<cart.length; i++){
      totalPrice+= parseFloat(cart[i].price) * parseFloat(cart[i].quantity);
  }
  return totalPrice;
}
function populateEditCart(){
  var cart = JSON.parse(sessionStorage.cart);
  var total = getTotalPrice();
  $('#editCartTable').empty();
  $('#totalPrice').text('P'+total.toFixed(2));
  for(var i=0; i<cart.length; i++){
    $('#editCartTable').append('<tr>'+
                                '<td class="col-sm-8 col-md-6">'+
                                    '<div class="media">'+
                                        '<div class="media-body">'+
                                            '<h4 class="media-heading" style="color:#00703c;">'+cart[i].docuName+'</h4>'+
                                            '<h5 class="media-heading" style="color:#00703c;"> degree: '+cart[i].degree+'</h5>'+
                                        '</div>'+
                                    '</div>'+
                                '</td>'+
                                '<td class="col-sm-1 col-md-1" style="text-align: center">'+
                                    '<p><span class="glyphicon glyphicon-minus" onclick="minusOrder('+cart[i].docuID+',\''+cart[i].degree+'\')"></span>&nbsp;&nbsp;&nbsp;'+cart[i].quantity+'&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-plus" onclick="plusOrder('+cart[i].docuID+',\''+cart[i].degree+'\')"></p>'+

                                '</td>'+
                                '<td class="col-sm-1 col-md-1 text-center"><strong>P'+cart[i].price+'</strong></td>'+
                                '<td class="col-sm-1 col-md-1 text-center"><strong>P'+cart[i].price * cart[i].quantity+'</strong></td>'+
                                '<td class="col-sm-1 col-md-1">'+
                                    '<button type="button" class="btn btn-danger" class = "removeFromCart" onclick="reloadRemoveCart('+cart[i].docuID+',\''+cart[i].degree+'\')">'+
                                        '<span class="glyphicon glyphicon-remove"></span> Remove'+
                                    '</button></td></tr>');
  }
}
