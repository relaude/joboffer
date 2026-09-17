export async function downloadPdf(url) {
    // Fetch in the user's browser so the endpoint receives the current session cookies.
    const response = await fetch(url, {
        credentials: "same-origin",
        redirect: "error",
        cache: "no-store",
        signal: AbortSignal.timeout(180000)
    });

    if (!response.ok || !response.headers.get("content-type")?.toLowerCase().startsWith("application/pdf")) {
        throw new Error("The server did not return a PDF.");
    }

    const pdf = await response.blob();
    const objectUrl = URL.createObjectURL(pdf);
    const link = document.createElement("a");
    try {
        link.href = objectUrl;
        link.download = "Job-Offer-Letter.pdf";
        document.body.appendChild(link);
        link.click();
    } finally {
        link.remove();
        // Give the browser time to start the download before releasing its contents.
        setTimeout(() => URL.revokeObjectURL(objectUrl), 60000);
    }
}
