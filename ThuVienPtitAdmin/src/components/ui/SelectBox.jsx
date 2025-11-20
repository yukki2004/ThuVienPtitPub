import React from "react";

const SelectBox = ({ semesters, selectedSemester, onChange }) => {
  return (
    <div className="mb-4">
      <label className="block text-sm font-medium mb-1">Chọn học kỳ:</label>
      <select
        className="border rounded p-2 w-full"
        value={selectedSemester || ""}
        onChange={(e) => onChange(e.target.value)}
      >
        <option value="">Tất cả học kỳ</option>
        {semesters.map((s) => (
          <option key={s.semester_id} value={s.semester_id}>
            {s.name} (Năm {s.year})
          </option>
        ))}
      </select>
    </div>
  );
};

export default SelectBox;
