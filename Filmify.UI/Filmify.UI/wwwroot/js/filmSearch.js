document.addEventListener('DOMContentLoaded', () => {
    const searchBtn = document.getElementById('filmSearchBtn');
    const searchInput = document.getElementById('filmSearchInput');
    const resultsContainer = document.getElementById('filmResultsContainer');
    let lastKey = 0;
    let currentSearch = '';
    // Check if we're on the GetAll page
    const isGetAllPage = window.location.pathname.includes('/Films/GetAll');
    const isHomePage = window.location.pathname === '/' || window.location.pathname.includes('/Home');
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
    function redirectToGetAll() {
        if (currentSearch.trim()) {
            window.location.href = `/Films/GetAll?searchText=${encodeURIComponent(currentSearch)}`;
        } else {
            window.location.href = '/Films/GetAll';
        }
    }


    searchBtn.addEventListener('click', () => {
        currentSearch = searchInput.value.trim();
        lastKey = 0;
        if (currentSearch === '') {
            if (isGetAllPage) {
                redirectToGetAll();
            } else {
                fetchResults();
            }
        }
        else {
            fetchResults();
        }
    });

    searchInput.addEventListener('keyup', (e) => {
        currentSearch = e.target.value.trim();
        lastKey = 0;
        if (currentSearch === '') {
            if (isGetAllPage) {
                redirectToGetAll();
            } else {
                fetchResults();
            }
        } else {
            fetchResults();
        }
        
    });
});
