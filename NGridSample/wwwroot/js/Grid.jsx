var GridHeader = React.createClass({
    setSort: function (e) {
        this.props.toggleSort(this.props.name, e.shiftKey)
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
        var style = {cursor: "pointer"}
        return (<th><div onClick={this.setSort} style={style}>{this.props.name}<span className={sortIcon} aria-hidden="true" /></div></th>)
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

    toggleSort: function (columnName, append) {
        var sortColumns = this.state.sortColumns;

        var column = sortColumns.find(function (item) {
            return (item.column === columnName)
        });
        
        if (column === undefined) {
            var newColumn = { column: columnName, sortDesc: false };
            if (!append) {
                sortColumns = [newColumn];
            }
            else {
                sortColumns.push(newColumn);
            }
        }
        else {
            column.sortDesc = !column.sortDesc;
            if (!append) {
                sortColumns = [column];
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

            var column = this.state.sortColumns.find(function (sortCol) {
                return (sortCol.column === item.name)
            });

            var sorted = column !== undefined;
            var sortedDesc = column !== undefined && column.sortDesc;
            return (<GridHeader key={index} name={item.name} sorted={sorted} sortedDesc={sortedDesc} toggleSort={this.toggleSort }/>)
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