var GridHeader = React.createClass({
    setSort: function () {
        this.props.toggleSort(this.props.name)
    },
    render: function () {
        if (this.props.sorted) {
            if (this.props.sortedDesc) {
                var sortIcon = "glyphicon glyphicon-sort-by-attributes-alt pull-right";
            }
            else {
                var sortIcon = "glyphicon glyphicon-sort-by-attributes pull-right";
            }
        }
        else {
            var sortIcon = "glyphicon glyphicon-sort pull-right";
        }
        return (<th>{this.props.name}<a href="#" onClick={this.setSort }><span className={sortIcon} aria-hidden="true" /></a></th>)
    }
});

var Grid = React.createClass({
    getInitialState: function () {

        return {
            columns: [],
            data: [],
            sortColumns: []
        };
    },

    toggleSort: function (columnName) {
        var sortColumns = this.state.sortColumns;

        var column = sortColumns.find(function (item) {
            return (item.column === columnName)
        });
        
        if (column === undefined) {
            sortColumns = [{ column: columnName, sortDesc: false }];
        }
        else {
            if (column.sortDesc) {
                var index = sortColumns.indexOf(column);
                sortColumns.splice(index, 1);
            }
            else if (!column.sortDesc) {
                column.sortDesc = true;
                column.sort = true;
            }
        }

        this.update({ sortColumns: sortColumns });
    },

    update: function(props) {
        this.serverRequest = $.post({ url: "/Home/GetData" }, props).success(function (result) {
            this.setState(result);
        }.bind(this));
    },

    componentDidMount: function () {
        this.update({});
    },
    render: function () {
        var columns = this.state.columns.map(function (item, index) {
            return (<GridHeader key={index} name={item.name} sorted={item.sorted} sortedDesc={item.sortedDesc} toggleSort={this.toggleSort }/>)
        }, this);

        var columnData = this.state.columns;

        var rows = this.state.data.map(function (item, index) {
            var cols = columnData.map(function (col, index) {
                var value = item[col.name];
                if (value === null || value === undefined)
                    value = "";
                return (<td key={index}>{value.toString()}</td>)
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