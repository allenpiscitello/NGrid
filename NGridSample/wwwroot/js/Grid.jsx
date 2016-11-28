var Grid = React.createClass({

    getInitialState: function() {
        return {
            Columns: [{ Name: "Column1" }, { Name: "Column2" }],
            Data: [{ Column1: "Item 1", Column2: "Item 2" }, { Column1: "Item 3", Column2: "Item 4" }]
        };
    },
    render: function () {
        var columns = this.state.Columns.map(function (item, index) {
            return (<th key={index}>{item.Name}</th>)
        });

        var columnData = this.state.Columns;

        var rows = this.state.Data.map(function (item, index) {
            var cols = columnData.map(function (col, index) {
                return (<td key={index}>{item[col.Name]}</td>)
            });
            return (<tr key={index}>{cols}</tr>)
        });

        return (
          <div className="table-responsive">
            <table className="table table-bordered table-striped">
                <thead>
                    <tr>
                    {columns}
                </tr>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
          </div>
      );
    }
});
ReactDOM.render(
  <Grid />,
  document.getElementById('content')
);