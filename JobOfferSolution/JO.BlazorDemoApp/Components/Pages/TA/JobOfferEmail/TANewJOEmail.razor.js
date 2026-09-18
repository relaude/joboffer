export function getSelectedOptionId(select) {
    return Number(select.selectedOptions[0].dataset.optionId);
}

export async function downloadAttachment(fileName, streamReference) {
    const bytes = await streamReference.arrayBuffer();
    const url = URL.createObjectURL(new Blob([bytes], { type: "application/pdf" }));
    const link = document.createElement("a");
    try {
        link.href = url;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
    } finally {
        link.remove();
        setTimeout(() => URL.revokeObjectURL(url), 60000);
    }
}
