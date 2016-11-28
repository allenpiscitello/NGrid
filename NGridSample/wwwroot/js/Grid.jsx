var Grid = React.createClass({

    getInitialState: function() {
        return { Columns: [{ Name: "Column1" }, { Name: "Column2" }] };
    },
    render: function () {
        var columns = this.state.Columns.map(function (item) {
            return (<th>{item.Name}</th>)
        })

        return (
          <div className="table-responsive">
            <table className="table table-bordered table-striped">
                <thead>
                    <tr>
                    {columns}
                </tr>
                </thead>
                <tbody>
                    <tr><td>item1</td><td>item2</td></tr>
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