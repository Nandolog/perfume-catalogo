(function () {
    window.compartirWhatsApp = function (texto, numero) {
    const base = (numero && numero.length > 0)
        ? `https://wa.me/${numero}`
        : 'https://wa.me/';
    const url = `${base}?text=${encodeURIComponent(texto)}`;
    window.open(url, '_blank');
};

    window.generarPDFProducto = function (producto) {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF({ unit: 'mm', format: 'a4' });

        doc.setFontSize(22);
        doc.text('Catalogo de Perfumes', 105, 20, { align: 'center' });

        doc.setFontSize(18);
        doc.text(producto.nombre, 20, 50);

        doc.setFontSize(11);
        doc.setTextColor(100);
        doc.text(`Categoria: ${producto.categoria}`, 20, 60);

        doc.setTextColor(0);
        doc.setFontSize(12);
        const descripcion = doc.splitTextToSize(producto.descripcion, 170);
        doc.text(descripcion, 20, 75);

        doc.setFontSize(16);
        doc.setTextColor(22, 163, 74);
        doc.text(`Precio: Gs. ${producto.precio.toLocaleString('es-PY')}`, 20, 75 + descripcion.length * 7 + 15);

        doc.setTextColor(0);
        doc.setFontSize(10);
        doc.text('Escribenos por WhatsApp para mas informacion.', 105, 280, { align: 'center' });

        doc.save(`${producto.nombre}.pdf`);
    };

   window.generarPDFCatalogo = function (data) {
    let productos = data;
    if (!Array.isArray(productos) && data && typeof data === 'object') {
        const keys = Object.keys(data);
        for (const k of keys) {
            if (Array.isArray(data[k])) {
                productos = data[k];
                break;
            }
        }
    }

    if (!Array.isArray(productos)) {
        console.error('generarPDFCatalogo: no se recibió un array', data);
        alert('Error generando PDF: datos inválidos');
        return;
    }

    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({ unit: 'mm', format: 'a4' });

    doc.setFontSize(24);
    doc.text('Catalogo de Perfumes', 105, 20, { align: 'center' });

    let y = 40;

    productos.forEach((p, i) => {
        if (y > 260) {
            doc.addPage();
            y = 20;
        }

        doc.setFontSize(14);
        doc.setTextColor(0);
        doc.text(`${i + 1}. ${p.nombre}`, 20, y);
        y += 7;

        doc.setFontSize(10);
        doc.setTextColor(100);
        doc.text(`${p.categoria || ''} - Gs. ${Number(p.precio).toLocaleString('es-PY')}`, 25, y);
        y += 6;

        doc.setTextColor(80);
        const desc = doc.splitTextToSize(p.descripcion || '', 165);
        doc.text(desc, 25, y);
        y += desc.length * 5 + 6;
    });

    doc.save('catalogo-perfumes.pdf');
};

    window.leerArchivoComoBytes = async function (inputId) {
        const inputEl = document.getElementById(inputId);
        if (!inputEl || !inputEl.files || inputEl.files.length === 0) {
            return null;
        }
        const file = inputEl.files[0];
        const buffer = await file.arrayBuffer();
        return {
            bytes: Array.from(new Uint8Array(buffer)),
            nombre: file.name
        };
    };

    window.limpiarInputArchivo = function (inputId) {
        const inputEl = document.getElementById(inputId);
        if (inputEl) inputEl.value = '';
    };

        // Puente sincrónico con localStorage para persistir la sesión de Supabase
    window.localStorageSync = {
        get: function (key) {
            return window.localStorage.getItem(key);
        },
        set: function (key, value) {
            window.localStorage.setItem(key, value);
        },
        remove: function (key) {
            window.localStorage.removeItem(key);
        }
    };

        // Animaciones al scroll con IntersectionObserver
    window.activarAnimacionesScroll = function () {
        const elementos = document.querySelectorAll('.fade-in:not(.visible)');
        if (elementos.length === 0) return;

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        });

        elementos.forEach(el => observer.observe(el));
    };
        // Escuchar el scroll y notificar al componente Blazor
    window.registrarScrollListener = function (dotNetRef) {
        let ticking = false;

        const handler = () => {
            if (!ticking) {
                window.requestAnimationFrame(() => {
                    dotNetRef.invokeMethodAsync('OnScroll', window.scrollY);
                    ticking = false;
                });
                ticking = true;
            }
        };

        window.addEventListener('scroll', handler, { passive: true });
    };
})();