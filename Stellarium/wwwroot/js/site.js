function toggleTheme() {
    const htmlTag = document.getElementsByTagName('html')[0]
    if (htmlTag.hasAttribute('data-theme')) {
        htmlTag.removeAttribute('data-theme')
        return window.localStorage.removeItem("site-theme")
    }

    htmlTag.setAttribute('data-theme', 'dark')
    window.localStorage.setItem("site-theme", "dark")
}

function applyInitialTheme() {
    const theme = window.localStorage.getItem("site-theme")
    if (theme !== null) {
        const htmlTag = document.getElementsByTagName("html")[0]
        htmlTag.setAttribute("data-theme", theme)
    }
}

const rusToLat = function (str) {
    let ru = {
        'а': 'a', 'б': 'b', 'в': 'v', 'г': 'g', 'д': 'd',
        'е': 'e', 'ё': 'e', 'ж': 'j', 'з': 'z', 'и': 'i',
        'к': 'k', 'л': 'l', 'м': 'm', 'н': 'n', 'о': 'o',
        'п': 'p', 'р': 'r', 'с': 's', 'т': 't', 'у': 'u',
        'ф': 'f', 'х': 'h', 'ц': 'c', 'ч': 'ch', 'ш': 'sh',
        'щ': 'shch', 'ы': 'y', 'э': 'e', 'ю': 'u', 'я': 'ya',
        'ъ': 'ie', 'ь': '', 'й': 'i'
    };
    let newString = [];

    return [...str].map(l => {
        let latL = ru[l.toLocaleLowerCase()];

        if (l !== l.toLocaleLowerCase()) {
            latL = latL.charAt(0).toLocaleUpperCase() + latL.slice(1);
        } else if (latL === undefined) {
            latL = l;
        }

        return latL;
    }).join('');
}

function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    //console.log('ID: ' + profile.getId());
    //console.log('Name: ' + profile.getName());
    //console.log('Image URL: ' + profile.getImageUrl());
    //console.log('Email: ' + profile.getEmail());
    //console.log('submited: ' + document.cookie.split('; ').find(row => row.startsWith('submited=')).split('=')[1]);
  
    document.cookie = "userimg=" + profile.getImageUrl();
    document.cookie = "useremail=" + profile.getEmail();

    var name = profile.getName().replace(/ /g, "_");

    console.log(rusToLat(name));

    var name = encodeURIComponent(rusToLat(profile.getName()));
    document.cookie = "username=" + name;
    window.localStorage.setItem('username', rusToLat(profile.getName().replace(/ /g, "_")));
    window.localStorage.setItem('userimg', profile.getImageUrl());
    window.localStorage.setItem('useremail', profile.getEmail());
    if (document.cookie.split('; ').find(row => row.startsWith('submited=')).split('=')[1] == "true") {
    }
    else {
        document.getElementById("form").submit();
        document.cookie = "submited=" + true;
    }
    document.getElementById("text").innerHTML = profile.getName();
}

function AdvancedSearchShow() {
    document.getElementById("advancedSearch").style.display = "block";
    document.getElementById("advancedSearchButton").style.display = "none";
}

window.onload = function OnLoad() {
    //alert(document.cookie.split('; '));
    //var username = document.cookie.split('; ').find(row => row.startsWith('username=')).split('=')[1];
    //var userimg = document.cookie.split('; ').find(row => row.startsWith('userimg=')).split('=')[1];
    //document.cookie = "visitorId=" + window.localStorage.getItem('visitorId');

    if (window.localStorage.getItem('userimg')) {
        document.cookie = "userimg=" + window.localStorage.getItem('userimg');
        document.cookie = "useremail=" + window.localStorage.getItem('useremail');
        document.cookie = "username=" + window.localStorage.getItem('username');
        window.localStorage.setItem('userid', document.cookie.split('; ').find(row => row.startsWith('userid=')).split('=')[1]);
        document.cookie = "userid=" + window.localStorage.getItem('userid');
    }
    if (window.localStorage.getItem('username')) {
        document.getElementById("login").innerHTML = window.localStorage.getItem('username').replace("_", " ");
        document.getElementById("userimage").src = window.localStorage.getItem('userimg');
        document.getElementById("userimage").display = "block"
        document.cookie = "userid=" + window.localStorage.getItem('userid');
    }
    else {
        document.getElementById("login").innerHTML = "Гость";
        document.getElementById("userimage").display = "none"
    }
    var visitorId = document.cookie.split('; ').find(row => row.startsWith('visitorId=')).split('=')[1];
    if (visitorId) {
        window.localStorage.setItem('visitorId', visitorId);
    }
    else
    {
        visitorId = window.localStorage.getItem('visitorId');
        document.cookie = "visitorId=" + visitorId +";expires=Thu, 01 Jan 2077 00:00:00 GMT";
    }
    visitorId = window.localStorage.getItem('visitorId');
    document.cookie = "visitorId=" + visitorId + ";expires=Thu, 01 Jan 2077 00:00:00 GMT";
    //запоминание сколбара через куки
}

function OnLoadAuth() {
    document.cookie = "submited=" + false;
}

function MessageText() {
    var elements = document.getElementsByName("answerText2");
    for (var i = 0; i < 500; i++) {
        elements[i].value = document.getElementById("messageText").value;
    }
}

applyInitialTheme();

document
    .getElementById("themetoggle")
    .addEventListener("click", toggleTheme);