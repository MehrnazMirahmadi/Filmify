document.addEventListener('DOMContentLoaded', () => {
    const searchBtn = document.getElementById('filmSearchBtn');
    const searchInput = document.getElementById('filmSearchInput');
    const resultsContainer = document.getElementById('filmResultsContainer');
    let lastKey = 0;
    let currentSearch = '';

    function fetchResults() {
        fetch(`/Films/Search?searchText=${encodeURIComponent(currentSearch)}&lastKey=${lastKey}`)
            .then(res => res.text())
            .then(html => {
                if (lastKey === 0) resultsContainer.innerHTML = '';
                resultsContainer.insertAdjacentHTML('beforeend', html);

                const loadMoreBtn = document.getElementById('loadMoreBtn');
                if (loadMoreBtn) {
                    lastKey = loadMoreBtn.dataset.lastKey;
                    loadMoreBtn.addEventListener('click', () => {
                        fetchResults();
                    });
                }
            });
    }

    searchBtn.addEventListener('click', () => {
        currentSearch = searchInput.value.trim();
        lastKey = 0;
        fetchResults();
    });

    searchInput.addEventListener('keyup', (e) => {
        currentSearch = e.target.value.trim();
        lastKey = 0;
        if (currentSearch === '') {
           
            if (window.location.pathname === '/Films/GetAll') {
                fetchResults();
            }
        } else {
            fetchResults();
        }
    });
});
