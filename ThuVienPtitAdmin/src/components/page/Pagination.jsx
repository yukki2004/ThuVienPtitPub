import ReactPaginate from "react-paginate";

const Pagination = ({ totalItem, pageSize, currentPage, onPageChange }) => {
  const pageCount = Math.ceil(totalItem / pageSize);

  return (
    <ReactPaginate
      pageCount={pageCount}
      marginPagesDisplayed={2}
      pageRangeDisplayed={3}
      onPageChange={(selected) => onPageChange(selected.selected + 1)}
      containerClassName="flex gap-2 justify-center mt-4"
      pageClassName="border p-2 rounded cursor-pointer"
      activeClassName="bg-blue-500 text-white"
      previousLabel="Prev"
      nextLabel="Next"
    />
  );
};

export default Pagination;
