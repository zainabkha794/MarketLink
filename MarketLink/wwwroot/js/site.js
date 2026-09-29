/* MarketLink · eGreen Basket — interactions */
(function () {
    "use strict";

    /* ── Sticky nav shadow ── */
    var navbar = document.getElementById("navbar");
    if (navbar) {
        window.addEventListener("scroll", function () {
            navbar.classList.toggle("scrolled", window.scrollY > 10);
        }, { passive: true });
    }

    /* ── Mobile nav ── */
    var toggle = document.getElementById("navToggle");
    var links = document.getElementById("navLinks");
    if (toggle && links) {
        toggle.addEventListener("click", function () {
            links.classList.toggle("open");
        });
    }

    /* ── User dropdown ── */
    var menu = document.getElementById("userMenu");
    var avatarBtn = document.getElementById("avatarBtn");
    if (menu && avatarBtn) {
        avatarBtn.addEventListener("click", function (e) {
            e.stopPropagation();
            menu.classList.toggle("open");
        });
        document.addEventListener("click", function () { menu.classList.remove("open"); });
    }

    /* ── Reveal on scroll ── */
    var revealEls = document.querySelectorAll(".reveal");
    if (revealEls.length && "IntersectionObserver" in window) {
        var io = new IntersectionObserver(function (entries) {
            entries.forEach(function (en) {
                if (en.isIntersecting) {
                    en.target.classList.add("visible");
                    io.unobserve(en.target);
                }
            });
        }, { threshold: 0.12 });
        revealEls.forEach(function (el) { io.observe(el); });
    } else {
        revealEls.forEach(function (el) { el.classList.add("visible"); });
    }

    /* ── Product image fallback ── */
    document.querySelectorAll("img[data-fallback]").forEach(function (img) {
        img.addEventListener("error", function () {
            if (img.dataset.failed) return;
            img.dataset.failed = "1";
            var fb = document.createElement("div");
            fb.className = "fallback";
            fb.textContent = img.dataset.fallback || "🌿";
            if (img.parentNode) img.parentNode.appendChild(fb);
            img.style.visibility = "hidden";
        });
    });

    /* ── Quantity steppers ── */
    document.querySelectorAll(".qty-stepper").forEach(function (stepper) {
        var input = stepper.querySelector("input");
        var minus = stepper.querySelector("[data-step='-1']");
        var plus = stepper.querySelector("[data-step='1']");
        if (!input) return;
        if (minus) minus.addEventListener("click", function () {
            input.value = Math.max(1, (parseInt(input.value, 10) || 1) - 1);
        });
        if (plus) plus.addEventListener("click", function () {
            input.value = (parseInt(input.value, 10) || 0) + 1;
        });
    });

    /* ── Auto-hide flash messages ── */
    setTimeout(function () {
        document.querySelectorAll(".flash").forEach(function (f) {
            f.style.transition = "opacity .6s ease, transform .6s ease";
            f.style.opacity = "0";
            f.style.transform = "translateY(-8px)";
            setTimeout(function () { f.remove(); }, 650);
        });
    }, 7000);

    /* ── Chatbot ── */
    var chatBody = document.getElementById("chatBody");
    if (chatBody) {
        var form = document.getElementById("chatForm");
        var input = document.getElementById("chatInput");

        function addMsg(role, text, time) {
            var wrap = document.createElement("div");
            wrap.className = "chat-msg " + role;
            wrap.textContent = text;
            var t = document.createElement("div");
            t.className = "chat-time";
            t.textContent = time || new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
            wrap.appendChild(t);
            chatBody.appendChild(wrap);
            chatBody.scrollTop = chatBody.scrollHeight;
        }

        function send(text) {
            if (!text.trim()) return;
            addMsg("user", text);
            var tokenInput = document.querySelector("#chatForm input[name='__RequestVerificationToken']");
            var body = new URLSearchParams();
            body.append("message", text);
            if (tokenInput) body.append("__RequestVerificationToken", tokenInput.value);

            fetch("/Chatbot/Ask", {
                method: "POST",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: body.toString()
            })
                .then(function (r) { return r.json(); })
                .then(function (data) { addMsg("bot", data.reply); })
                .catch(function () { addMsg("bot", "Something went wrong — please try again in a moment."); });
        }

        if (form) {
            form.addEventListener("submit", function (e) {
                e.preventDefault();
                var text = input.value;
                input.value = "";
                send(text);
            });
        }

        document.querySelectorAll(".chat-chip").forEach(function (chip) {
            chip.addEventListener("click", function () { send(chip.textContent.trim()); });
        });

        window.sendChat = send;
    }

    /* ── Leaflet map bootstrap ── */
    window.initMap = function (elementId, points, zoom) {
        var el = document.getElementById(elementId);
        if (!el || typeof L === "undefined") return null;
        if (!points || !points.length) return null;
        var lat = points.reduce(function (s, p) { return s + p.lat; }, 0) / points.length;
        var lng = points.reduce(function (s, p) { return s + p.lng; }, 0) / points.length;
        var map = L.map(elementId, { scrollWheelZoom: false }).setView([lat, lng], zoom || 11);
        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            maxZoom: 18,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        var goldIcon = L.icon({
            iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
            iconRetinaUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png",
            shadowUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png",
            iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41]
        });

        points.forEach(function (p) {
            L.marker([p.lat, p.lng], { icon: goldIcon })
                .addTo(map)
                .bindPopup("<strong>" + (p.title || "MarketLink") + "</strong><br/>" + (p.subtitle || ""));
        });
        return map;
    };
})();
