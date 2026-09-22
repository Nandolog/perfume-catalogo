window.compartirWhatsApp = (texto) => {
    const url = `https://wa.me/?text=${encodeURIComponent(texto)}`;
    window.open(url, '_blank');
};

window.generarPDFProducto = (producto) => {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({ unit: 'mm', format: 'a4' });

    doc.setFontSize(22);
    doc.text('🌸 Catálogo de Perfumes', 105, 20, { align: 'center' });

    doc.setFontSize(18);
    doc.text(producto.nombre, 20, 50);

    doc.setFontSize(11);
    doc.setTextColor(100);
    doc.text(`Categoría: ${producto.categoria}`, 20, 60);

    doc.setTextColor(0);
    doc.setFontSize(12);
    const descripcion = doc.splitTextToSize(producto.descripcion, 170);
    doc.text(descripcion, 20, 75);

    doc.setFontSize(16);
    doc.setTextColor(22, 163, 74);
    doc.text(`Precio: Gs. ${producto.precio.toLocaleString('es-PY')}`, 20, 75 + descripcion.length * 7 + 15);

    doc.setTextColor(0);
    doc.setFontSize(10);
    doc.text('Escríbenos por WhatsApp para más información.', 105, 280, { align: 'center' });

    doc.save(`${producto.nombre}.pdf`);
};

window.generarPDFCatalogo = (productos) => {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({ unit: 'mm', format: 'a4' });

    doc.setFontSize(24);
    doc.text('🌸 Catálogo de Perfumes', 105, 20, { align: 'center' });

    let y = 40;
    doc.setFontSize(12);

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
        doc.text(`${p.categoria} — Gs. ${p.precio.toLocaleString('es-PY')}`, 25, y);
        y += 6;

        doc.setTextColor(80);
        const desc = doc.splitTextToSize(p.descripcion, 165);
        doc.text(desc, 25, y);
        y += desc.length * 5 + 6;
    });

    doc.save('catalogo-perfumes.pdf');

    window.leerArchivoComoByte = async (input) => {
    const input = document.getElementById('archivoInput');
    if (!input || !input.files || input.files.length === 0) {
        return null;
    }
    const file = input.files[0];
    const buffer = await file.arrayBuffer();
    return {
        byte : Array.from(new Uint8Array(buffer)),
        nombre: file.name,
    };
};

window.limparInputArchivo = (inputId) => {
    const input = document.getElementById(inputId);
    if (input) input.value = '';
};
};