$(document).ready(function() {


    //1'
    var ajaxFormSubmit = function() {
        var $form = $(this);

        //dakle ovde smo pokupili formu sa atributom data-otf-ajax='true', kod koje je iniciran submit event
        //Posto se nalazimo unutar te forme dobijamo je sa this

        //Iz te forme koju smo pokupili mozemo da pokupimo i sve opcije, url je action atribut, type je metod - get,
        //data - podaci koji se salju serveru, oni se serijalizuju i kupe i salju serveru preko tog data

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
            //podaci se salju u name - value parovima i post-uju se
        };

        //preko options govorimo ajaxu gdje da pozove, sta da uradi, get ili post i slicno. i on to radi asinhrono.
        //I kada je zahtjev gotov poziva se callback function i rezultat je u data funkciji
        $.ajax(options).done(function (data) {
            //ovim pribavljamo dom element koji je potrebno updejtovati. u ovom slucaju je to element sa data-otf-target atributom #restaurantList
            var $target = $($form.attr("data-es-target"));
            //$target.html(data);
            $target.replaceWith(data);
        });
        return false;
    }

    //3'
    //Mozemo koristiti bilo koji paged list. samo u viewu specificirati koji doom atribut zelimo updejtovati.
    var getPage = function() {
        var $a = $(this); //Pokupimo anchor tag na koji smo kliknuli

        var options = {
            url: $a.attr("href"),
            data: $("form").serialize(), //ovo dodajemo da uzima u obzir i search
            type: "get"
        };

        $.ajax(options).done(function(data) {
            var target = $a.parents("div.pagedList").attr("data-es-target");
            $(target).replaceWith(data);
        });
        return false;
    };


    //2''

    var submitAutoCompleteForm = function(event, ui) {
        var $input = $(this);

        $input.val(ui.item.label);
        //Postavljamo vrijednost inputa nakon sto je selektovan u autocomplete formi

        var $form = $input.parents("form:first"); //Pronadji prvu roditeljsku formu
        $form.submit(); //Pozivamo submit event
    }
     

    //2'
    var createAutocomplete = function () {
        //Kupimo input koji je inicirao dogadjaj
        var $input = $(this);

        var options = {
            //Source mu govori gdje da pokupi podatke, pokupice url preko data-otf-autocomplete atributa
            source: $input.attr("data-es-autocomplete"),
            select: submitAutoCompleteForm
        };
        $input.autocomplete(options);
    };



    //1
    $("form[data-es-ajax='true']").submit(ajaxFormSubmit);

    //Potrebno je uvatiti zahtjeve za paged listu :)
    //3
    //Dakle prosledjujemo delagat, kada se klikne na glavnu povrsinu, konkretno na link paged liste izvrsi getPage :)
    $(".body-content").on("click", ".pagedList a", getPage);

    //2 autocomplete. Pronadji sve input elemente sa datim atributom i povezi ih na event
    $("input[data-es-autocomplete]").each(createAutocomplete);
});
